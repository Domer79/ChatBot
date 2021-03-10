import { Component, OnInit } from '@angular/core';
import {MatIconRegistry} from "@angular/material/icon";
import {DomSanitizer} from "@angular/platform-browser";

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit {

  constructor(
      private matIconRegistry: MatIconRegistry,
      private domSanitizer: DomSanitizer,
  ) {
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
    this.matIconRegistry.addSvgIcon("operators",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/operators.svg"));
    this.matIconRegistry.addSvgIcon("questions",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/questions.svg"));
    this.matIconRegistry.addSvgIcon("reports",
        this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/reports.svg"));
  }

  ngOnInit(): void {
  }

}
