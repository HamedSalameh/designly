import { DOCUMENT } from '@angular/common';
import { ComponentFactoryResolver, Inject, Injectable, Injector, TemplateRef } from '@angular/core';
import { ModalComponent } from '../components/modal-component/modal-component.component';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  private modalNotifier?: Subject<string>;

  constructor(private componentFactoryResolver: ComponentFactoryResolver, 
    private injector: Injector,
    @Inject(DOCUMENT) private document: Document)
  {

  }

  open(content: TemplateRef<any>, config?: {
    title?: string;
    content?: string;
  }) {
    
    const modalComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalComponent);
    const contentViewRef = content.createEmbeddedView(null);
    const modalComponent = modalComponentFactory.create(this.injector, [contentViewRef.rootNodes]);

    modalComponent.instance.title = config?.title;
    modalComponent.instance.content = config?.content;

    modalComponent.instance.closeEvent.subscribe(() => {
      this.closeModal();
    });
    modalComponent.instance.confirmEvent.subscribe(() => {
      this.confirmModal();
    });

    modalComponent.hostView.detectChanges();

    this.document.body.appendChild(modalComponent.location.nativeElement);
    this.modalNotifier = new Subject();
    return this.modalNotifier?.asObservable();
  }

  closeModal() {
    this.modalNotifier?.complete();
  }

  confirmModal() {
    this.modalNotifier?.next('confirm');
    this.closeModal();
  }
}
