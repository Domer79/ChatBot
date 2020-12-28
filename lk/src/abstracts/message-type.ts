export enum MessageType{
  String,
  File
}

export enum MessageStatus{
  // Отправляется
  sending,

  // Доставлено
  saved,

  // Получено
  received
}

export enum MessageOwner{
  client,
  operator
}

export enum PageEvent{
  update
}

