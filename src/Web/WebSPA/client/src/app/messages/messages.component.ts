import { Component, OnInit } from '@angular/core';
import { IMessage } from '../models/message';
import { MessageService } from '../services/message.service';
import { OffsetPagination } from './../models/OffsetPagination';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  messages: IMessage[] = [];
  paginationMessages: OffsetPagination<IMessage[]>;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  loading = false;

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }
  loadMessages(): void {
    this.loading = true;
    this.messageService
      .getMessages(this.pageNumber, this.pageSize, this.container)
      .subscribe((respnse) => {
        this.messages = respnse.data;
        this.paginationMessages = respnse;
        this.loading = false;
      });
  }

  pageChanged(event: any): void {
    this.pageNumber = event.page;
    this.loadMessages();
  }

  deleteMessage(messageId: number): void {
    this.messageService.deleteMessage(messageId).subscribe(() => {
      this.messages.splice(
        this.messages.findIndex((m) => m.id === messageId),
        1
      );
    });
  }
}
