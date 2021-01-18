import {Component, OnInit} from '@angular/core';
import Question from "../../../abstracts/Question";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {EditQuestionDialogComponent} from "../edit-question-dialog/edit-question-dialog.component";
import {Observable} from "rxjs";
import {QuestionService} from "../../services/question.service";
import {MessageBoxService} from "../../services/message-box.service";
import {DialogResult} from "../../../abstracts/DialogResult";
import {Security} from "../../security.decorator";

@Component({
  selector: 'app-questions-page',
  templateUrl: './questions-page.component.html',
  styleUrls: ['./questions-page.component.sass']
})
@Security('QuestionManager')
export class QuestionsPageComponent implements OnInit {
  questions: Observable<Question[]>

  constructor(
      private dialog: MatDialog,
      private questionService: QuestionService,
      private messageBoxService: MessageBoxService
  ) {
    this.questions = this.questionService.getAll();
  }

  ngOnInit(): void {
  }

  openEditQuestionDialog(question: Question | undefined) {
    const matConfig = new MatDialogConfig();
    matConfig.width = '900px';
    matConfig.height = '700px'
    matConfig.data = question;
    this.dialog.open(EditQuestionDialogComponent, matConfig).afterClosed().subscribe(result => {
      if (result === "ok") {
        this.questions = this.questionService.getAll();
      }
    });
  }

  questionEdit(question: Question) {
    this.openEditQuestionDialog(question);
  }

  async questionDelete(question: Question): Promise<void> {
    this.messageBoxService.confirm(`Подтвердите удаление вопроса ${question.number}`).subscribe(async result => {
      if (result === DialogResult.No)
        return;

      if (result === DialogResult.Yes){
        await this.questionService.deleteQuestion(question);
        this.questions = this.questionService.getAll();
      }
    })
  }
}
