import { Component, OnInit } from '@angular/core';
import {Security} from "../security.decorator";

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.sass']
})
@Security('Developer')
export class TestComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
