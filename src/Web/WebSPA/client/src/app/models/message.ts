export interface IMessage {
  id: number;
  senderId: number;
  recipientId: number;
  content: string;
  dateRead?: Date;
  messageSent: Date;
}
