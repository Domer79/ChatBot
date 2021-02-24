import {LinkType} from "../app/contracts/message-dialog";

export interface DialogFilterData{
    linkType: LinkType;
    startDate: Date;
    closeDate: Date;
    operator: string;
    client: string;
    dialogNumber: number;
}
