import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import MessageDialog from "../../contracts/message-dialog";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {QuestionService} from "../../services/question.service";
import {Observable} from "rxjs";
import Question from "../../../abstracts/Question";
import {ok} from "assert";

@Component({
  selector: 'app-edit-question-dialog',
  templateUrl: './edit-question-dialog.component.html',
  styleUrls: ['./edit-question-dialog.component.sass']
})
export class EditQuestionDialogComponent implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  questionParentId: string;
  questionsUnlessChild: Observable<Question[]>;

  constructor(
      private questionService: QuestionService,
      private _formBuilder: FormBuilder,
      public dialogRef: MatDialogRef<EditQuestionDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: Question,
  ) {
    if (!this.data) {
      this.data = new Question();
    }

    this.questionsUnlessChild = this.questionService.getAllQuestionsUnlessChild(this.data.id);
  }

  ngOnInit(): void {
    this.firstFormGroup = this._formBuilder.group({
      questionCtrl: ['', Validators.required],
      responseCtrl: ['', Validators.required],
    });
    this.secondFormGroup = this._formBuilder.group({
      questionParentId: ['', Validators.nullValidator]
    });

    this.firstFormGroup.controls.questionCtrl.setValue(this.data.question);
    this.firstFormGroup.controls.responseCtrl.setValue(this.data.response);
    this.secondFormGroup.controls.questionParentId.setValue(this.data.parentId);
  }

  async saveQuestion(): Promise<void> {
    this.data.question = this.firstFormGroup.controls.questionCtrl.value;
    this.data.response = this.firstFormGroup.controls.responseCtrl.value;
    this.data.parentId = this.secondFormGroup.controls.questionParentId.value;

    const regex = /<link>(.+)<\/link>/i;
    if (this.data.response.match(regex)){
      debugger;
      this.data.response = this.data.response.replace(regex, '<a href="$1" target="_blank">$1</a>')
    }

    await this.questionService.saveQuestion(this.data);
    this.dialogRef.close('ok');
  }
}
