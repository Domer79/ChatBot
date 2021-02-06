export interface Settings{
  id: string;
  name: string;
  description: string;
  value: string;
  dateCreated: Date;
}

export interface NumberSettings extends Settings{
  numberValue: number;
}

export interface Shift{
  begin: number;
  close: number;
}
