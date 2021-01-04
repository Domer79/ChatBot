import { Injectable } from '@angular/core';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@microsoft/signalr';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private tokenConnection: HubConnection;
  private resolve: any;
  private reject: any;
  private token: string | null = null;
  private token$: Promise<string> = new Promise<string>((resolve, reject) => {
    if (this.token) {
      resolve(this.token);
    }
    else{
      this.resolve = resolve;
      this.reject = reject;
    }
  });

  constructor() {
    this.tokenConnection = new HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/token`)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    this.tokenConnectionInit();

    this.tokenConnection.start()
      .then(r => {
        console.log('TokenConnection started');

        this.tokenConnection.invoke('GetToken')
          .then(v => console.log(`Call GetToken: ${v}`))
          .catch(e => this.reject(e));
      })
      .catch(err => this.reject(err));
  }

  private tokenConnectionInit(): void{
    this.tokenConnection.on('setToken', (tokenId: string) => {
      this.token = tokenId;
      this.resolve(tokenId);
    });
  }

  getToken(): Promise<string>{
    return this.token$;
  }
}
