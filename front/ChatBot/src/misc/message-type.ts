export enum MessageType{
  String,
  File,
  Question,
  ButtonForForm,
  CloseSession,
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
  operator,
  chatbotHelper,
}

export enum PageEvent{
  update
}

export enum TimeStatus{
  None,
  JustNow,
  Today,
  Yesterday,
}
