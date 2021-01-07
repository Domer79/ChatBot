import Fio from "../../abstracts/Fio";

export default class User implements Fio{
    constructor(options: any) {
        this.id = options.id;
        this.login = options.loginId;
        this.firstName = options.firstName;
        this.lastName = options.lastName;
        this.middleName = options.middleName;
        this.email = options.email;
    }
    id: string;
    login: string;
    email: string;
    firstName: string;
    lastName: string;
    middleName: string;
}
