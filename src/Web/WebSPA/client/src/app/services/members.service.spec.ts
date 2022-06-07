import { HttpParams, HttpRequest } from '@angular/common/http';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MemberFilter } from '../models/memberFilter';
import { environment } from './../../environments/environment';
import { paginatedmembers } from './../../mock/paginationMembers';
import { MembersService } from './members.service';

describe('members.service', () => {
  let httpTestingController: HttpTestingController;
  let memberService: MembersService;
  const baseUrl = environment.baseUrl;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MembersService],
    });

    httpTestingController = TestBed.inject(HttpTestingController);
    memberService = TestBed.inject(MembersService);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(memberService).toBeTruthy();
  });

  it('get members should get paginated members', () => {
    const memberFilter = new MemberFilter();
    memberFilter.pageNumber = 1;
    memberFilter.pageSize = 5;
    let params = new HttpParams();
    params = params.append('pageNumber', memberFilter.pageNumber.toString());
    params = params.append('pageSize', memberFilter.pageSize.toString());
    params = params.append('maxAge', memberFilter.maxAge.toString());
    params = params.append('minAge', memberFilter.minAge.toString());
    params = params.append('orderBy', memberFilter.orderBy);
    if (memberFilter.gender) {
      params = params.append('gender', memberFilter.gender);
    }
    memberService.getMembers(memberFilter).subscribe((response) => {
      expect(response.currentPage).toEqual(1);
    });

    const httpReq = new HttpRequest('GET', `${baseUrl}Member/GetMembers`, {
      params,
    });
    console.log(httpReq.urlWithParams);
    const req = httpTestingController.expectOne(httpReq.urlWithParams);

    expect(req.request.method).toEqual('GET');
    expect(req.request.params.get('pageNumber')).toEqual('1');
    req.flush(paginatedmembers);
  });
});
