import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Security} from "../security.decorator";
import {ReportsService} from "../services/reports.service";

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.sass']
})
@Security('ReportsPage')
export class ReportsComponent implements OnInit {
  range = new FormGroup({
    start: new FormControl('', Validators.required),
    end: new FormControl('', Validators.required)
  });

  constructor(
      private reportsService: ReportsService
  ) { }

  ngOnInit(): void {
  }

  getReport() {
    this.reportsService.getReport(this.range.value);
  }
}
