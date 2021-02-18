export default class Question{
  constructor(options){
    this.parentId = options.parentId
  }
  id: string;
  number: string;
  question: string;
  response: string;
  parentId: string;
}
