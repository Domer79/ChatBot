import { Component, OnInit } from '@angular/core';
import {Security} from "../security.decorator";

@Component({
  selector: 'app-dialogs',
  templateUrl: './dialogs.component.html',
  styleUrls: ['./dialogs.component.sass']
})
@Security('DialogPage')
export class DialogsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
