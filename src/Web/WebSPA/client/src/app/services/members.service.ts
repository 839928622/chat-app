import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IOffsetPagination } from '../models/IOffsetPagination';

import { IMember } from '../models/member';
import { MemberFilter } from '../models/memberFilter';
import { IPhoto } from '../models/photo';

// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user')).token
//   })
// };

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.baseUrl;
  paginationResult: IOffsetPagination<IMember[]>;
  constructor(private http: HttpClient) { }

  getMembers(memberFilter: MemberFilter): Observable<IOffsetPagination<IMember[]>> {
     let params = this.producePaginationHeaders(memberFilter.pageNumber, memberFilter.pageSize);

     params = params.append('maxAge', memberFilter.maxAge.toString());
     params = params.append('minAge', memberFilter.minAge.toString());
     params = params.append('orderBy', memberFilter.orderBy);
     if (memberFilter.gender) {
      params = params.append('gender', memberFilter.gender);
     }

     return this.http.get<IOffsetPagination<IMember[]>>(this.baseUrl + 'Member/GetMembers', {params}).pipe(
      map(response => {
        return response;
      })
    );
  }

  private producePaginationHeaders(pageNumber: number, pageSize: number): HttpParams {
    let params = new HttpParams();
      // params.append('pageNumber', pageNumber.toString()); == http://localhost/users?pageNumber=pageNumber
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
  }
  getSingleMember(userId: string): Observable<IMember>{
    return this.http.get<IMember>(this.baseUrl + 'Member/' + userId);
  }

  updateMember(member: IMember): Observable<unknown> {
    return this.http.put(this.baseUrl + 'users', member);
  }

  setMainPhoto(photoId: number): Observable<object>{
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number): Observable<object> {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string): Observable<object> {
    return this.http.post(this.baseUrl + 'like/' + username, {});
  }

  getLikes(predicate: string): Observable<Partial<IMember[]>>{
    return this.http.get<Partial<IMember[]>>(this.baseUrl + 'like?predicate=' + predicate);
  }

  /**
   * get all user photos by user id
   * @param userid  user identifier
   * @returns An Array that containing the  user photos
   */
   getPhotosByUserId(userid: number): Observable<IPhoto[]>
   {
     return this.http.get<IPhoto[]>(this.baseUrl + 'member/UserPhotos/' + userid);
   }
}
