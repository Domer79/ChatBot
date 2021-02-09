import User from "./user";

export default class MessageDialog{
    constructor(options: any) {
        this.id = options.id;
        this.number = options.number;
        this.dateCreated = options.dateCreated;
        this.dateWork = options.dateWork;
        this.dateCompleted = options.dateCompleted;
        this.dialogStatus = options.dialogStatus;
        this.operatorId = options.operatorId;
        this.firstMessage = options.firstMessage;
        this.clientId = options.clientId;
    }
    id: string;
    number: number;
    dateCreated: Date;
    dateWork?: Date;
    dateCompleted?: Date;
    dialogStatus: DialogStatus;
    operatorId?: string;
    firstMessage?: string;
    clientId?: string;
    client: User;
    operator: User;
}

export enum DialogStatus{
    /*
    * Старт диалога, когда пользователь отправляет первое сообщение
    * */
    Started = 1,

    /*
    * Оператор берет в работу или первый раз отвечает
    */
    Active = 1 << 1,

    /*
    * Соединение с пользователем прервано, но диалог может быть возобновлен
    * */
    Sleep = 1 << 2,

    /*
    * Диалог закрыт
    * */
    Closed = 1 << 3,

    /*
    * Диалог отклонен оператором
    * */
    Rejected = 1 << 4,
}

export enum LinkType {
    all = DialogStatus.Started | DialogStatus.Active | DialogStatus.Rejected | DialogStatus.Closed,
    opened = DialogStatus.Started,
    rejected = DialogStatus.Rejected,
    worked = DialogStatus.Active,
    closed = DialogStatus.Closed,
}
