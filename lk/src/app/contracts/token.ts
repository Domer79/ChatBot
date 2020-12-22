export default class token{
    constructor(options: any){
        this.tokenId = options.tokenId;
        this.dateCreated = options.dateCreated;
        this.dateExpired = options.dateExpired;
        this.autoExpired = options.autoExpired;
        this.userId = options.userId;
    }

    tokenId: string;
    dateCreated: Date;
    dateExpired: Date;
    autoExpired: Date;
    userId: string;
}
