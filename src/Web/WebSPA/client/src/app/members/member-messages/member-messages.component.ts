import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { IMessage } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() messages: IMessage[];

  @Input() currentUserMainPhotoUrl?: string;
  @Input() anotherUserMainPhotoUrl?: string;

  @Input() currentUserId: number;
  @Input() userIdThatIamTalkingTo: number;
  messageContent: string;
  recentMessageBetweenTwoMembers$: Observable<IMessage[]>;
  constructor(private messageService: MessageService, private toastr: ToastrService,
              ) { }

  ngOnInit(): void {
    // this.loadMessages();
    this.recentMessageBetweenTwoMembers$ = this.messageService.recentMessage$;
    // loading conversation(messages) between two users
    this.messageService.getRecentMessagesBetweenTwoUsers(this.userIdThatIamTalkingTo + '')
    .subscribe(offsetPaginatedMessage => {
     this.messageService.publishMessagesToMessageSource(offsetPaginatedMessage.data);
    });
  }

  // loadMessages(): void {
  //   this.messageService.getMessageThread(this.username).subscribe(messages => {
  //     this.messages = messages;
  //   });
  // }

  sendMessage(): void{
    if (this.messageContent === undefined || this.messageContent.length === 0) {
      this.toastr.error('You are not allowed to send an empty message');
      return;
    }
    this.messageService.sendMessage(this.userIdThatIamTalkingTo, this.messageContent)
    .then(() => {
      this.messageForm.reset();
      // after sending a new messages,then will assume previous messages are read
      this.messageService.markMessagesAsRead(this.userIdThatIamTalkingTo);
    });
  //   .subscribe(message => {
  //     this.messages.push(message);
  //     this.messageForm.reset();
  //   });
  }
}
