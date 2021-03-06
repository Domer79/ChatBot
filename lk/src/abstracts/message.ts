import {MessageOwner, MessageStatus, MessageType} from "./message-type";

export default interface Message{
  id: string;
  content: string;
  type: MessageType;
  owner: MessageOwner;
  time: Date;
  status: MessageStatus;
  messageDialogId: string;
  sender?: string;
}
