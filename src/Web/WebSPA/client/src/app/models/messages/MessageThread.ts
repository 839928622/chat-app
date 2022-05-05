import { BasePagination } from '../BasePagination';
/**
 * get message(s) between two users
 */
export class MessageThread extends BasePagination {
  /** user that I am talking to */
  AnotherUserUserId: number;
}
