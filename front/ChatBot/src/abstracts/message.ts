import {MessageType} from '../misc/message-type';

export default interface Message{
  type: MessageType;

  content: string;

  isClient: boolean;
}
