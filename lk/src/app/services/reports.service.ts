import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Moment} from "moment";
import Helper from "../misc/Helper";

@Injectable({
  providedIn: 'root'
})
export class ReportsService {

  constructor(
      private http$: HttpClient
  ) { }

  getReport(reportParams: {'start': Moment, 'end': Moment}): void{
    debugger
    const params = {
      start: reportParams.start.utc().format(),
      end: reportParams.end.utc().format()
    };
    // @ts-ignore
    this.http$.get<Blob>('api/Reports/GetReport', {params: { ...params }, observe: "response", responseType: 'blob' as 'json'})
        .subscribe((response) => {
          console.log(response);
          debugger;
          const fileName = `Report_${reportParams.start.local().format('DD.MM.YYYY')}-${reportParams.end.local().format('DD.MM.YYYY')}.xlsx`;
          Helper.downLoadFile(fileName, response, 'application/ms-excel')
    });
  }
}
