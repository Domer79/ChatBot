import Fio from "../../abstracts/Fio";

export default class User implements Fio{
    constructor(options: any | undefined = undefined) {
        if (options){
            this.id = options.id;
            this.login = options.login;
            this.firstName = options.firstName;
            this.lastName = options.lastName;
            this.middleName = options.middleName;
            this.email = options.email;
            this.isActive = options.isActive;
            this.isOperator = options.isOperator;
            this.dateCreated = options.dateCreated;
        }
    }
    id: string;
    login: string;
    email: string;
    firstName: string;
    lastName: string;
    middleName: string;
    isActive: boolean;
    isOperator: boolean;
    dateCreated: Date;
}
