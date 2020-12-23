import { Injectable } from '@angular/core';
import Question from '../../abstracts/Question';
import {Observable, of} from 'rxjs';
import {TokenService} from './token.service';

@Injectable({
  providedIn: 'root'
})
export class QuestionsProviderService {
  private selectedQuestions: Question[] = [];

  private questions: Question[] = [
    { id: '1', question: 'Оформление заявки и пакет документов', response: 'Для г. Новосибирск, г. Обь, п. Пашино, сельсоветы Новосибирского района (кроме Боровского, Плотниковского, Станционного, Ярковского) \n' +
        'либо указан район Новосибирской области, кроме Новосибирского, величина максимальной мощности  150 кВт (включительно) и I категория надежности электроснабжения, а также свыше 150 кВт независимо от категории надежности электроснабжения:\n' +
        '\n' +
        'Центр обслуживания клиентов \n' +
        ' г. Новосибирск, ул. Якушева, 16А, БЦ «Триумф» \n' +
        ' \n' +
        '\n' +
        'Для районов Новосибирской области (за исключением населенных пунктов, указанных в предыдущем пункте настоящей таблицы) или Боровской, Плотниковский, Станционный и Ярковский сельсоветы Новосибирского района, максимальная мощность энергопринимающих устройств до 150 кВт (включительно), II и (или) III категории надежности электроснабжения:\n' +
        '\n' +
        'Пункты обслуживания клиентов соответствующих филиалов по населенным пунктам/районам согласно вкладке адреса/контакты ЦОК, ПОКов' },
    { id: '3', question: 'Вопрос2', response: 'Ответ 2' },
    { id: '4', question: 'Вопрос3', response: 'Ответ 3' },
    { id: '2', question: 'Вопрос4', response: 'Ответ 4' },
    { id: '5', question: 'Вопрос5', response: 'Ответ 5' },
    { id: '6', question: 'Вопрос6', response: 'Ответ 6' },
    { id: '3', question: 'Вопрос2', response: 'Ответ 2' },
    { id: '4', question: 'Вопрос3', response: 'Ответ 3' },
    { id: '2', question: 'Вопрос4', response: 'Ответ 4' },
    { id: '5', question: 'Вопрос5', response: 'Ответ 5' },
    { id: '6', question: 'Вопрос6', response: 'Ответ 6' },
    { id: '3', question: 'Вопрос2', response: 'Ответ 2' },
    { id: '4', question: 'Вопрос3', response: 'Ответ 3' },
    { id: '2', question: 'Вопрос4', response: 'Ответ 4' },
    { id: '5', question: 'Вопрос5', response: 'Ответ 5' },
    { id: '6', question: 'Вопрос6', response: 'Ответ 6' },
    { id: '3', question: 'Вопрос2', response: 'Ответ 2' },
    { id: '4', question: 'Вопрос3', response: 'Ответ 3' },
    { id: '2', question: 'Вопрос4', response: 'Ответ 4' },
    { id: '5', question: 'Вопрос5', response: 'Ответ 5' },
    { id: '6', question: 'Вопрос6', response: 'Ответ 6' },
    { id: '3', question: 'Вопрос2', response: 'Ответ 2' },
    { id: '4', question: 'Вопрос3', response: 'Ответ 3' },
    { id: '2', question: 'Вопрос4', response: 'Ответ 4' },
    { id: '5', question: 'Вопрос5', response: 'Ответ 5' },
    { id: '6', question: 'Вопрос6', response: 'Ответ 6' },
  ];

  constructor() { }

  getQuestions(): Observable<Question[]>{
    return of(this.questions);
  }

  showQuestionResponse(question: Question): void {
    this.selectedQuestions.push(question);
  }

  stopShowResponse(): void{
    if (this.selectedQuestions.length === 0)
    {
      throw new Error('Can\'t stop showing because there\'s nothing to show');
    }

    this.selectedQuestions.pop();
  }

  checkSelectedQuestion(): boolean {
    return this.selectedQuestions.length !== 0;
  }

  getSelectedQuestion(): Question{
    if (this.selectedQuestions.length === 0)
    {
      throw new Error('Can\'t stop showing because there\'s nothing to show');
    }

    return this.selectedQuestions[this.selectedQuestions.length - 1];
  }
}
