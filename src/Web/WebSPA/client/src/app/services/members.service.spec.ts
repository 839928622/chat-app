import { environment } from './../../environments/environment';
import { TestBed } from '@angular/core/testing';
import { MembersService } from './members.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('members.service', () => {

  let httpTestController: HttpTestingController;
  let memberService: MembersService;
  const baseUrl = environment.baseUrl;
  beforeEach(() => {
    TestBed.configureTestingModule(
      {
        imports: [HttpClientTestingModule],
        providers: [MembersService]
      }
    );

    httpTestController = TestBed.inject(HttpTestingController);
    memberService = TestBed.inject(MembersService);
  });

  it('should be created', () => {
    expect(memberService).toBeTruthy();
  });

});
