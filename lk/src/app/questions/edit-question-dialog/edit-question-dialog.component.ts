import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import MessageDialog from "../../contracts/message-dialog";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {QuestionService} from "../../services/question.service";
import {Observable} from "rxjs";
import Question from "../../../abstracts/Question";
import {ok} from "assert";
import {DialogResult} from "../../../abstracts/DialogResult";

@Component({
  selector: 'app-edit-question-dialog',
  templateUrl: './edit-question-dialog.component.html',
  styleUrls: ['./edit-question-dialog.component.sass']
})
export class EditQuestionDialogComponent implements OnInit {
  firstFormGroup: FormGroup;

  constructor(
      private _formBuilder: FormBuilder,
      public dialogRef: MatDialogRef<EditQuestionDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: Question,
  ) {
    if (!this.data) {
      throw new Error('Passed question data does not initialized')
    }
  }

  ngOnInit(): void {
    this.firstFormGroup = this._formBuilder.group({
      questionCtrl: ['', Validators.required],
      responseCtrl: ['', Validators.required],
    });

    this.firstFormGroup.controls.questionCtrl.setValue(this.data.question);
    this.firstFormGroup.controls.responseCtrl.setValue(this.data.response);
  }

  saveQuestion(): void {
    this.data.question = this.firstFormGroup.controls.questionCtrl.value;
    this.data.response = this.firstFormGroup.controls.responseCtrl.value;

    const regex = /<link>(.+)<\/link>/i;
    if (this.data.response.match(regex)){
      this.data.response = this.data.response.replace(regex, '<a href="$1" target="_blank">$1</a>')
    }

    this.dialogRef.close(this.data);
  }

  no(): void {
    this.dialogRef.close(DialogResult.No);
  }
}
