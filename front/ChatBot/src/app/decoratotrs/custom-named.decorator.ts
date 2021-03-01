export function PageComponent(options: { 'customName': string, 'iconFile': string }){
  return (constructor: any) => {
    Object.defineProperty(constructor, '__customName', {
      enumerable: false,
      configurable: false,
      writable: false,
      value: options.customName
    });

    Object.defineProperty(constructor, '__iconFile', {
      enumerable: false,
      configurable: false,
      writable: false,
      value: options.iconFile
    });
  };
}
