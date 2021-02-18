import {Component, OnDestroy, OnInit} from '@angular/core';
import Question from "../../../abstracts/Question";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {EditQuestionDialogComponent} from "../edit-question-dialog/edit-question-dialog.component";
import {Observable, Subscription} from "rxjs";
import {QuestionService} from "../../services/question.service";
import {MessageBoxService} from "../../services/message-box.service";
import {DialogResult} from "../../../abstracts/DialogResult";
import {Security} from "../../security.decorator";
import { v4 as uuidv4 } from 'uuid';
import { NIL as guidEmpty } from 'uuid';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-questions-page',
  templateUrl: './questions-page.component.html',
  styleUrls: ['./questions-page.component.sass']
})
@Security('QuestionManager')
export class QuestionsPageComponent implements OnInit, OnDestroy {
  questions: Observable<Question[]>
  private paramsSubscription: Subscription;
  private parentId: string;

  constructor(
      private dialog: MatDialog,
      private questionService: QuestionService,
      private messageBoxService: MessageBoxService,
      private route: ActivatedRoute,
      private router: Router
  ) {
  }

  ngOnInit(): void {
    this.paramsSubscription = this.route.params.subscribe(param => {
      this.parentId = param['parentId'] ?? guidEmpty;
      this.questions = this.questionService.getQuestions(this.parentId);
    })
  }

  openEditQuestionDialog(question: Question | undefined): void {
    const matConfig = new MatDialogConfig();
    matConfig.width = '900px';
    matConfig.height = '700px'
    matConfig.data = question;
    this.dialog.open(EditQuestionDialogComponent, matConfig).afterClosed().subscribe(async result => {
      if (result === DialogResult.No) {
        return;
      }
      else if (typeof result === 'object'){
        await this.questionService.saveQuestion(result);
      }
    });
  }

  async saveQuestion(question: Question): Promise<void>{
    await this.questionService.saveQuestion(question);
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
        this.questions = this.questionService.getQuestions(this.parentId);
      }
    })
  }

  ngOnDestroy(): void {
    this.paramsSubscription.unsubscribe();
  }

  goToChild(question: Question) {
    this.router.navigate(['/questions/', question.id]).then();
  }

  async addNewQuestion(): Promise<void> {
    await this.questionService.saveQuestion(new Question({parentId: this.parentId}));
    this.questions = this.questionService.getQuestions(this.parentId);
  }
}
