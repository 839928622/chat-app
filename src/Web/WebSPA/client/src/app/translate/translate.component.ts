import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TimeagoIntl } from 'ngx-timeago';
import {strings as englishStrings} from 'ngx-timeago/language-strings/en';
import {strings as ChineseStrings} from 'ngx-timeago/language-strings/zh-CN';
@Component({
  selector: 'app-translate',
  templateUrl: './translate.component.html',
  styleUrls: ['./translate.component.css']
})
export class TranslateComponent implements OnInit {
  langs = {
    'en-US': 'English',
    'zh-CN': '中文简体'
  };
  constructor(private translate: TranslateService, private intl: TimeagoIntl) {
    translate.addLangs(['en-US', 'zh-CN']);
    translate.setDefaultLang('zh-CN');
    const browerLanguage = translate.getBrowserLang();
    // console.log(browerLanguage);
    switch (browerLanguage) {
     case 'en':
        translate.use('en-US');
        this.intl.strings = englishStrings;
        break;
     case 'zh':
        translate.use('zh-CN');
        this.intl.strings = ChineseStrings;
        break;
     default:
        translate.use('en-US');
        this.intl.strings = englishStrings;
        break;
   }
   }

  ngOnInit(): void {
  }

  useLanguage(language: string): void {
    switch (language) {
      case 'en-US':
        this.translate.use('en-US');
        this.intl.strings = englishStrings;
        break;
      case 'zh-CN':
          this.translate.use('zh-CN');
          this.intl.strings = ChineseStrings;
          break;
      default:
        this.translate.use('en-US');
        this.intl.strings = englishStrings;
        break;
    }
    this.intl.changes.next();
  }
}
