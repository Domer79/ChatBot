import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import Question from "../../../abstracts/Question";
import User from "../../contracts/user";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-operator-edit-dialog',
  templateUrl: './operator-edit-dialog.component.html',
  styleUrls: ['./operator-edit-dialog.component.sass']
})
export class OperatorEditDialogComponent implements OnInit {
  action: string;
  userEditStepGroup: FormGroup;
  passwordGroup: FormGroup;

  constructor(
      public dialogRef: MatDialogRef<OperatorEditDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: User,
      private _formBuilder: FormBuilder,
  ) { }

  ngOnInit(): void {
    this.action = this.data ? 'Реактирование' : 'Создание';
    this.userEditStepGroup = this._formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      middleName: ['', Validators.nullValidator],
      login: ['', Validators.required],
      email: ['', Validators.required],
    });
  }

  get userInfoIsSubmit(){
    const controls = this.userEditStepGroup.controls;
    for (const prop in controls){
      if (controls[prop].status === "INVALID")
        return false
    }

    return true;
  }

  userInfoSave() {
    console.log(this.userEditStepGroup.controls);
  }
}
