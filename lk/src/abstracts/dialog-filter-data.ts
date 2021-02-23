import {DialogStatus} from "../app/contracts/message-dialog";

export interface DialogFilterData{
    dialogStatus: DialogStatus;
    startDate: Date;
    closeDate: Date;
    operator: string;
    client: string;
    dialogNumber: number;
}
