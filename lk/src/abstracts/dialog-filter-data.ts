import {LinkType} from "../app/contracts/message-dialog";

export interface DialogFilterData{
    linkType: LinkType;
    startDate: string;
    closeDate: string;
    operator: string;
    client: string;
    dialogNumber: number;
}
