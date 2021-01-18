export default class MainMenu{
    private static pages: PageInfo[] = [
        {
            link: '/dialogs/27',
            name: 'Диалоги',
            code: 'dialogs'
        },
        {
            link: '/questions',
            name: 'Вопросы',
            code: 'questions'
        },
        {
            link: '/operators',
            name: 'Операторы',
            code: 'operators'
        }
    ];

    public static getPageInfo(code: string): PageInfo{
        const pages = MainMenu.pages.filter(pi => pi.code === code);
        if (pages.length === 0)
            throw new Error(`Page with code ${code} not found`);

        return pages[0];
    }
}

export interface PageInfo{
    link: string,
    name: string,
    code: string;
}
