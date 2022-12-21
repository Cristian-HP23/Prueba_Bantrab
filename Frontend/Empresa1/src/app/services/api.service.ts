import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/enviroment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor( private http : HttpClient) { }

  postEmpresa(data : any){
    return this.http.post<any>(environment.URL,data,{headers:{'user':environment.User,'password':environment.pass}})
  }

  getEmpresas(){
    return this.http.get<any>(environment.URL,{headers:{'user':environment.User,'password':environment.pass}})
  }
  getEmpresa(id : number){
    return this.http.get<any>("ur")
  }

  putEmpresa(data:any, id:number){
    return this.http.put<any>(environment.URL+id,data,{headers:{'user':environment.User,'password':environment.pass}})
  }

  deleteEmpres(id:number){
    return this.http.delete<any>(environment.URL+id,{headers:{'user':environment.User,'password':environment.pass}})
  }
}
