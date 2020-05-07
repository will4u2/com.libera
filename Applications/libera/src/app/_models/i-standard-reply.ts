import { HttpResponse } from '@angular/common/http';

export interface IStandardReply {
  success: boolean;
  response: HttpResponse<any>;
  messages: string[];
  exceptions: any[];
}
