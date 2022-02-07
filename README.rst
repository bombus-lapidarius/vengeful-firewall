~~~~~~~~~~~~~~~~~
Vengeful Firewall
~~~~~~~~~~~~~~~~~

**A transparent encryption layer implementation for IPFS in .NET (Core)**

Currently, the project only has a generic (and punny) code name to avoid any
potential for copyright infringement until a permanent naming solution is found.


Project goal
============

Provide an easy to use symmetric encryption layer that can be used as a proxy
between client applications and a conventional IPFS node running on the local
machine (possibly on remote machines too in the future).


General concept
===============

Files or raw data blocks handed over to the encryption layer shall be
transparently encrypted and stored in the backing IPFS network.

Similarly, any requests for data from the backing IPFS network shall be silently
replaced by a lookup of the CID associated with the raw encrypted binary content
corresponding to the plaintext file or raw data block that the request CID
refers to. The encryption layer will then fetch the encrypted content and return
the decrypted version instead.

All configuration shall happen out-of-band, i.e. the software shall emulate a
conventional IPFS node to any client applications accessing it via API or RPC
with encryption specific options being set via some well-known API or RPC
endpoints.


Status
======

The project is in a very early stage. Currently the core functionality for the
encryption and the decryption of data blocks is being implemented.

The current goal is supporting aes-cbc-essiv:sha256 (128, 192 and 256 bit keys),
followed by the replication of the minimal amount of IPFS functionality needed
to split files into blocks and to store encrypted versions of those blocks via
the underlying IPFS network.


Licensing
=========

Dual-licensed under MIT and Apache-2.0, by way of the `Permissive License Stack
<https://protocol.ai/blog/announcing-the-permissive-license-stack/>`_.

Apache-2.0: https://www.apache.org/licenses/license-2.0
MIT: https://www.opensource.org/licenses/mit
