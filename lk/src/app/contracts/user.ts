export default class User{
    constructor(options: any | undefined = undefined) {
        if (options){
            this.id = options.id;
            this.number = options.number;
            this.login = options.login;
            this.fio = options.fio;
            this.email = options.email;
            this.phone = options.phone;
            this.isActive = options.isActive;
            this.isOperator = options.isOperator;
            this.dateCreated = options.dateCreated;
            this.dateBlocked = options.dateBlocked;
        }
    }
    id: string;
    number: number;
    login: string;
    fio: string;
    email: string;
    phone: string;
    isActive: boolean;
    isOperator: boolean;
    dateCreated: Date;
    dateBlocked?: Date;
}
