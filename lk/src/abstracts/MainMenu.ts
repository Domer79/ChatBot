export default class MainMenu{
    public static Dialogs: PageInfo = {
        link: '/dialogs',
        name: 'Диалоги',
        code: 'dialogs'
    };
    public static Questions: PageInfo = {
        link: '/questions',
        name: 'Вопросы',
        code: 'questions'
    };
}

export interface PageInfo{
    link: string,
    name: string,
    code: string;
}
