import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { GetRecentMessagesParams } from '../models/messages/GetRecentMessagesParams';
import { IUser } from '../models/user';
import { IMessage } from './../models/message';
import { OffsetPagination } from './../models/OffsetPagination';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = environment.baseUrl;
  hubUrl = environment.hubUrl;
  private HubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<IMessage[]>([]);
  recentMessage$ = this.messageThreadSource.asObservable();
  constructor(private http: HttpClient, private busyService: BusyService) {}
  /**
   * connection establishment
   * @param currentUser current user
   * @param userIdThatIamTalkingTo user's userId That I am Talking To
   */
  createHubConnection(
    currentUser: IUser,
    userIdThatIamTalkingTo: number
  ): void {
    this.busyService.busy();
    this.HubConnection = new HubConnectionBuilder()
      .withUrl(
        this.hubUrl +
          'message?userIdThatIamTalkingTo=' +
          userIdThatIamTalkingTo,
        {
          accessTokenFactory: () => currentUser.token,
        }
      )
      .withAutomaticReconnect()
      .build();

    this.HubConnection.start()
      .catch((error) => console.log(error))
      .finally(() => {
        this.busyService.idle();
      });

    // listening
    // this.HubConnection.on('ReceiveMessageThread', (messages: IMessage[]) => {
    //   console.log('messagesThread', messages);
    //   this.messageThreadSource.next(messages);
    // });
    // new message
    this.HubConnection.on('NewMessage', (singleNewMessage: IMessage) => {
      console.log('NewMessage', singleNewMessage);
      this.recentMessage$.pipe(take(1)).subscribe((oldMessages) => {
        // merge old messages and new single message
        // optimization: we may use 'push' method, add new message to the tail of array
        const mergedArray = [...oldMessages, singleNewMessage].sort((a, b) =>
          a.messageSent > b.messageSent ? 1 : -1
        ); // asc
        this.messageThreadSource.next(mergedArray);
      });
    });

    // listening on  'MarkMessagesAsRead' event   userId means this user mark messages as read
    this.HubConnection.on('MarkMessagesAsRead', (userId: number) => {
      this.recentMessage$.pipe(take(1)).subscribe((oldMessages) => {
        oldMessages.forEach((message) => {
          if (message.dateRead === null && message.recipientId === userId) {
            message.dateRead = new Date(Date.now());
          }
        });
        this.messageThreadSource.next([...oldMessages]);
      });
    });
  }

  stopHubConnection(): void {
    if (this.HubConnection) {
      this.messageThreadSource.next([]);
      this.HubConnection.stop();
    }
  }
  getMessages(
    pageNumber: number,
    pageSize: number,
    container: string
  ): Observable<OffsetPagination<IMessage[]>> {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('Container', container);
    // https://www.angularjswiki.com/httpclient/get-params/
    return this.http.get<OffsetPagination<IMessage[]>>(
      this.baseUrl + 'messages',
      { params }
    );
  }

  /**
   * get getRecentMessages between two users
   * @param anotherUserId  the user that you are talking to
   */
  getRecentMessagesBetweenTwoUsers(
    anotherUserId: string
  ): Observable<OffsetPagination<IMessage[]>> {
    const messageParams = new GetRecentMessagesParams();
    messageParams.AnotherUserUserId = anotherUserId;
    // messageParams.pageNumber = 5;
    const httpParams = new HttpParams()
      .append('pageNumber', messageParams.pageNumber.toString())
      .append('pageSize', messageParams.pageSize.toString())
      .append('AnotherUserId', messageParams.AnotherUserUserId);
    return this.http.get<OffsetPagination<IMessage[]>>(
      this.baseUrl + 'message/RecentMessages',
      { params: httpParams }
    );
  }

  async sendMessage(
    userIdThatIamTalkingTo: number,
    content: string
  ): Promise<any> {
    // return this.http.post<IMessage>(this.baseUrl + 'messages', {recipientUsername: username, content});
    try {
      return this.HubConnection.invoke('SendMessage', {
        RecipientUserId: userIdThatIamTalkingTo,
        content,
      });
    } catch (error) {
      return console.log(error);
    }
  }

  deleteMessage(messageId: number): Observable<object> {
    return this.http.delete<object>(this.baseUrl + 'messages/' + messageId);
  }

  /**
   * publish messages to source for other componets to subscribe
   * @param messages An Array that comtaining  message(s)
   */
  publishMessagesToMessageSource(messages: IMessage[]): void {
    console.log('message', messages);
    this.messageThreadSource.next(
      messages.sort((a, b) => (a.messageSent > b.messageSent ? 1 : -1))
    ); // asc
  }

  /**
   * mark messages as Read
   * @param UserIdThatIamTalkingTo user's  Id That Iam Talking To
   * @returns void. nothing will be returnd
   */
  public markMessagesAsRead(UserIdThatIamTalkingTo: number): void {
    try {
      this.HubConnection.invoke('MarkMessagesAsRead', UserIdThatIamTalkingTo);
    } catch (error) {
      return console.log(error);
    }
  }
}
