import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import {
  TranslateLoader,
  TranslateModule,
  TranslateService,
} from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { paginatedmembers } from 'src/mock/paginationMembers';
import { MembersService } from './../../services/members.service';
import { MemberListComponent } from './member-list.component';

const translations: any = { CARDS_TITLE: 'This is a test' };
class FakeLoader implements TranslateLoader {
  getTranslation(lang: string): Observable<any> {
    return of(translations);
  }
}

describe('member-list.component', () => {
  let component: MemberListComponent;
  let fixture: ComponentFixture<MemberListComponent>;
  let translate: TranslateService;
  beforeEach(async () => {

    // Create a fake MembersService object with a `getMembers()` spy
    const membersService = jasmine.createSpyObj('MembersService', ['getMembers']);
    // Make the spy return a synchronous Observable with the test data
    const getMembersSpy = membersService.getMembers.and.returnValue(of(paginatedmembers));
    await TestBed.configureTestingModule({
      imports: [
        TranslateModule.forRoot({
          loader: { provide: TranslateLoader, useClass: FakeLoader },
        }),
        FormsModule
      ],
      declarations: [MemberListComponent],
      providers: [{ provide: MembersService, useValue: membersService }],
    }).compileComponents();

    fixture = TestBed.createComponent(MemberListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    translate = TestBed.inject(TranslateService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
