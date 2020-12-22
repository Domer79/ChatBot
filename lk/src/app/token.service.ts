import { Injectable } from '@angular/core';
import Token from "./contracts/token";

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private token: Token = null;
  exist: boolean = false;

  constructor() {
    this.refreshToken();
  }

  get tokenId(): string | null{
    if (!this.exist)
      return null;

    return this.token.tokenId;
  }

  saveToken(token: Token | string): void {
    if (!token)
      throw new Error('Argument of token null');

    let json;
    if (token instanceof Token)
      json = JSON.stringify(token);
    else if (typeof(token) == 'string')
      json = token;

    localStorage.setItem("token", json);
    this.refreshToken();
  }

  private refreshToken(): void{
    const token = localStorage.getItem("token");
    if (!token) return;

    this.exist = true;
    this.token = new Token(JSON.parse(token));
  }
}
