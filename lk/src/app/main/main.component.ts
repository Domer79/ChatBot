import {Component, OnInit} from '@angular/core';
import {MatIconRegistry} from "@angular/material/icon";
import {DomSanitizer} from "@angular/platform-browser";
import {Security} from "../security.decorator";
import {ActivatedRoute, Router} from "@angular/router";
import MainMenu, {PageInfo} from "../../abstracts/MainMenu";
import {TokenService} from "../services/token.service";
import {CacheService} from "../services/cache.service";
import {Observable, of} from "rxjs";
import {AuthService} from "../services/auth.service";

@Component({
  selector: 'main',
  templateUrl: './main.component.html',
  styleUrls:['./main.component.sass']
})
@Security("MainPage")
export class MainComponent implements OnInit {
  title = 'lk';
  activePageInfo: PageInfo = MainMenu.getPageInfo('dialogs');
  dialogsCanShow: Observable<boolean> = of(false);

  constructor(
      private matIconRegistry: MatIconRegistry,
      private domSanitizer: DomSanitizer,
      private router: Router,
      private tokenService: TokenService,
      private cacheService: CacheService,
      private route: ActivatedRoute,
      private auth: AuthService
  ){
    this.matIconRegistry.addSvgIcon("res-chat",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/res_chat.svg"));
    this.matIconRegistry.addSvgIcon("res-logo",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/ao_res_logo.svg"));
    this.matIconRegistry.addSvgIcon("res-menu-active",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/menu_active.svg"));
    this.matIconRegistry.addSvgIcon("res-exit",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/exit.svg"));
    this.matIconRegistry.addSvgIcon("filter",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/filter.svg"));
    this.matIconRegistry.addSvgIcon("arrow-down",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/arrow_down.svg"));
    this.matIconRegistry.addSvgIcon("calendar",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/calendar.svg"));
    this.matIconRegistry.addSvgIcon("lens",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/lens.svg"));
  }

  async ngOnInit(): Promise<void> {
    this.dialogsCanShow = this.auth.checkAccessPolicy('DialogPage');
  }

  async goto(pageCode: string): Promise<void> {
    this.activePageInfo = MainMenu.getPageInfo(pageCode);
    await this.router.navigate([this.activePageInfo.link]);
  }

  logout() {
    this.tokenService.clear();
    this.cacheService.clear();
    this.router.navigate(['/login'], {
      relativeTo: this.route,
    });
  }
}

