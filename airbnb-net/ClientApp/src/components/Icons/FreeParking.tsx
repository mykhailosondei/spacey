import * as React from "react";
import type { SVGProps } from "react";
const SvgFreeParking = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      height: 24,
      width: 24,
      fill: "currentcolor",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M26 19a1 1 0 1 1-2 0 1 1 0 0 1 2 0M7 18a1 1 0 1 0 0 2 1 1 0 0 0 0-2m20.7-5 .41 1.12A4.97 4.97 0 0 1 30 18v9a2 2 0 0 1-2 2h-2a2 2 0 0 1-2-2v-2H8v2a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-9c0-1.57.75-2.96 1.89-3.88L4.3 13H2v-2h3v.15L6.82 6.3A2 2 0 0 1 8.69 5h14.62a2 2 0 0 1 1.87 1.3L27 11.15V11h3v2zM6 25H4v2h2zm22 0h-2v2h2zm0-2v-5a3 3 0 0 0-3-3H7a3 3 0 0 0-3 3v5zm-3-10h.56L23.3 7H8.69l-2.25 6zm-15 7h12v-2H10z" />
  </svg>
);
export default SvgFreeParking;
