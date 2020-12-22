export default class User{
    constructor(options: any) {
        this.userId = options.userId;
        this.login = options.loginId;
        this.firstName = options.firstName;
        this.lastName = options.lastName;
        this.email = options.email;
    }
    userId: string;
    login: string;
    email: string;
    firstName: string;
    lastName: string;
}
