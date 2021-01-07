import {Component} from '@angular/core';
import {MatIconRegistry} from "@angular/material/icon";
import {DomSanitizer} from "@angular/platform-browser";
import {Security} from "../security.decorator";
import {LinkType} from "../contracts/message-dialog";
import {Router} from "@angular/router";

@Component({
  selector: 'main',
  templateUrl: './main.component.html',
  styleUrls:['./main.component.sass']
})
@Security("MainPage")
export class MainComponent {
  title = 'lk';
  activeLink = LinkType.all;
  linkType = LinkType;

  constructor(
      private matIconRegistry: MatIconRegistry,
      private domSanitizer: DomSanitizer,
      private router: Router,
  ){
    this.matIconRegistry.addSvgIcon("res-chat",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/res_chat.svg"));
    this.matIconRegistry.addSvgIcon("res-logo",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/ao_res_logo.svg"));
    this.matIconRegistry.addSvgIcon("res-menu-active",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/menu_active.svg"));
  }

  async goTo(linkType: LinkType) {
    debugger
    switch (linkType) {
      case LinkType.all:{
        this.activeLink = LinkType.all;
        await this.router.navigate(['dialogs'], { skipLocationChange: true });
        break;
      }
      case LinkType.opened:{
        this.activeLink = LinkType.opened;
        await this.router.navigate([`dialogs/${LinkType.opened}`], { skipLocationChange: true });
        break;
      }
      case LinkType.worked:{
        this.activeLink = LinkType.worked;
        await this.router.navigate([`dialogs/${LinkType.worked}`], { skipLocationChange: true });
        break;
      }
      case LinkType.rejected:{
        this.activeLink = LinkType.rejected;
        await this.router.navigate([`dialogs/${LinkType.rejected}`], { skipLocationChange: true });
        break;
      }
      case LinkType.closed:{
        this.activeLink = LinkType.closed;
        await this.router.navigate([`dialogs/${LinkType.closed}`], { skipLocationChange: true });
        break;
      }
    }
  }
}

