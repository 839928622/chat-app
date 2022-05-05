import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IGroup } from '../models/group';
import { PaginatedResult } from '../models/IPagination';
import { IMessage } from '../models/message';
import { MessageThread } from '../models/messages/MessageThread';
import { IUser } from '../models/user';
import { BusyService } from './busy.service';
import { getPaginationHeader, getPaginationResult } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.baseUrl;
  hubUrl = environment.hubUrl;
  private HubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<IMessage[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  constructor(private http: HttpClient, private busyService: BusyService) { }

  createHubConnection(currentUser: IUser): void {
    this.busyService.busy();
    this.HubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl, {
      accessTokenFactory: () => currentUser.token,
    }).withAutomaticReconnect().build();

    this.HubConnection.start().catch(error => console.log(error)).finally(
      () => {
        this.busyService.idle();
      }
    );

    // listening
    // this.HubConnection.on('ReceiveMessageThread', (messages: IMessage[]) => {
    //   console.log('messagesThread', messages);
    //   this.messageThreadSource.next(messages);
    // });
    // new message
    this.HubConnection.on('NewMessage', (singleNewMessages: IMessage) => {
      this.messageThread$.pipe(take(1)).subscribe(oldMessages => {
        // merge old messages and new single message
        this.messageThreadSource.next([...oldMessages, singleNewMessages ]);
      });
    });

     // listening on  'MarkMessagesAsRead' event
    this.HubConnection.on('MarkMessagesAsRead', () => {
        this.messageThread$.pipe(take(1)).subscribe(oldMessages => {
          oldMessages.forEach(message => {
            if (message.dateRead === null) {
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
  getMessages(pageNumber: number, pageSize: number, container: string): Observable<PaginatedResult<IMessage[]>>{
    let params = getPaginationHeader(pageNumber, pageSize);
    params = params.append('Container', container);

    return getPaginationResult<IMessage[]>(this.baseUrl + 'messages', params, this.http);
  }

/**
 * step into message thread and server will retur 5 recent messages by  "ReceiveMessageThread" event
 * @param anotherUserId  the user that you are talking to
 */
  getMessageThread(anotherUserId: number): void {
    try {
      const messageParams = new  MessageThread();
      messageParams.AnotherUserUserId = anotherUserId;
      this.HubConnection.invoke('MessageThread', messageParams);
   } catch (error) {
      console.log(error);
   }
  }

  async sendMessage(username: string, content: string): Promise<any>{
    // return this.http.post<IMessage>(this.baseUrl + 'messages', {recipientUsername: username, content});
    try {
      return this.HubConnection.invoke('SendMessage', { recipientUsername: username, content });
    } catch (error) {
      return console.log(error);
    }
  }



  deleteMessage(messageId: number): Observable<object> {
    return this.http.delete<object>(this.baseUrl + 'messages/' + messageId);
  }
}
