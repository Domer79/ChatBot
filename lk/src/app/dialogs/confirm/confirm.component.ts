import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import Question from "../../../abstracts/Question";
import {DialogResult} from "../../../abstracts/DialogResult";

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.sass']
})
export class ConfirmComponent implements OnInit {
  dialogMessage: string;
  dialogResult: DialogResult;

  constructor(
      public dialogRef: MatDialogRef<ConfirmComponent>,
      @Inject(MAT_DIALOG_DATA) public data: string,
  ) {
    this.dialogMessage = data ? data : 'Подтвердите действие';
  }

  ngOnInit(): void {
  }

  yes() {
    this.dialogRef.close(DialogResult.Yes);
  }

  no() {
    this.dialogRef.close(DialogResult.No);
  }
}
