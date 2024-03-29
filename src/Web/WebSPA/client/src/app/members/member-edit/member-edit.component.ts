import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { IMember } from 'src/app/models/member';
import { IUser } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('userProfileForm') userProfileForm: NgForm;
  member: IMember;
  currentUser: IUser;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any): void {
   if (this.userProfileForm.dirty) {
     $event.returnValue = true;
   }
  }
  constructor(private accountService: AccountService, private memberService: MembersService,
              private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.currentUser = user;
    });
   }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(): void {
    this.memberService.getSingleMember(this.currentUser.userId).subscribe(member => {
      this.member = member;
    });
  }

  updateMember(): void {
    this.memberService.updateMember(this.member).subscribe(() => {
      this.userProfileForm.reset(this.member);
    });

  }

}
