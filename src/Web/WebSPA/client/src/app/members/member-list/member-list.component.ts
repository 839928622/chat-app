import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { IMember } from 'src/app/models/member';
import { MemberFilter } from 'src/app/models/memberFilter';
import { OffsetPagination } from 'src/app/models/OffsetPagination';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: IMember[];
  pagination: OffsetPagination<IMember[]>;
  pageNumber = 1;
  pageSize = 5;
  memberFilter = new MemberFilter();
  genderOptions = [
    { value: 'male', display: 'member-list.males' },
    { value: 'female', display: 'member-list.females' },
    { value: 'others', display: 'member-list.others' },
  ];
  constructor(
    private memberService: MembersService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers(): void {
    this.memberService.getMembers(this.memberFilter).subscribe((response) => {
      this.members = response.data;
      this.pagination = response;
    });
  }

  pageChanged($event): void {
    this.memberFilter.pageNumber = $event.page;
    // this.pageSize = $event.itemsPerPage;
    this.loadMembers();
  }

  resetFilters(): void {
    this.memberFilter = new MemberFilter();
    this.loadMembers();
  }
}
