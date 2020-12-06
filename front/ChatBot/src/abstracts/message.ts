import {MessageOwner, MessageStatus, MessageType} from '../misc/message-type';

export default interface Message{
  type: MessageType;
  content: string;
  owner: MessageOwner;
  time: Date;
  status: MessageStatus;
}
