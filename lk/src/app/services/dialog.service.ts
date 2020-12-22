import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private httpClient: HttpClient) { }


}
