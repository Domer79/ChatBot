export enum MessageType{
  String,
  File
}

export enum MessageStatus{
  // Отправляется
  sending,

  // Доставлено
  delivered,

  // Получено
  received
}

export enum MessageOwner{
  client,
  operator
}
