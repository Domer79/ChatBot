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
      private router: Router,
      private tokenService: TokenService,
      private cacheService: CacheService,
      private route: ActivatedRoute,
      private auth: AuthService
  ){}

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

